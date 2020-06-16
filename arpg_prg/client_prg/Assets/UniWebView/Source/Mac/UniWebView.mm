//
//  UniWebView.m
//  UniWebView
//
//  Created by 王 巍 on 13-9-23.
//  Copyright (c) 2013年 王 巍. All rights reserved.
//

#import <WebKit/WebKit.h>
#import <unistd.h>
#import <Carbon/Carbon.h>
#import <OpenGL/gl.h>
#import "IUnityInterface.h"
#import "IUnityGraphics.h"

#define REFRESH_COUNT 40

typedef void (* UnityCommandCallback)(const char *gameObjectName, const char *methodName, const char *parameter);

static UnityCommandCallback lastCallback = NULL;

extern "C" {
    void _ConnetCallback(UnityCommandCallback callbackName);
    void _CallMethod(const char *gameObjectName, const char *methodName, const char *parameter);
}


void _ConnetCallback(UnityCommandCallback callbackName) {
    lastCallback = callbackName;
}

void _CallMethod(const char *gameObjectName, const char *methodName, const char *parameter) {
    if (lastCallback != NULL) {
        lastCallback(gameObjectName, methodName, parameter);
    }
}

typedef NS_ENUM(NSInteger, UniWebViewTransitionEdge) {
    UniWebViewTransitionEdgeNone,
    UniWebViewTransitionEdgeTop,
    UniWebViewTransitionEdgeLeft,
    UniWebViewTransitionEdgeBottom,
    UniWebViewTransitionEdgeRight
};

@interface UniWebView : WebView
@property (nonatomic, copy) NSString *currentURL;
@property (nonatomic, strong) NSMutableDictionary *headers;
@property (nonatomic, strong) NSBitmapImageRep *bitmap;
@property (nonatomic, assign) int textureId;
@property (nonatomic, strong) NSMutableArray *messageSchemes;
@property (nonatomic, assign) int webViewId;
@property (nonatomic, assign) BOOL openLinkInExternalBrowser;
@end

@implementation UniWebView

-(instancetype) init {
    self = [super init];
    if (self) {
        _currentURL = nil;
        _headers = [NSMutableDictionary dictionary];
        _bitmap = nil;
        _textureId = -1;
        _messageSchemes = [NSMutableArray array];
        _webViewId = -1;
    }
    return self;
}

@end

@interface UniWebViewManager : NSObject
{
    NSMutableDictionary *_webViewDic;
    NSString *_customUserAgent;
}
@end

@interface UniWebViewManager()<WebPolicyDelegate, WebFrameLoadDelegate>
@property (nonatomic, copy) NSString *customUserAgent;
@end

@implementation UniWebViewManager

static int webviewId = 300000;
static int webviewRenderCounter = 0;

@synthesize customUserAgent = _customUserAgent;

+ (UniWebViewManager *) sharedManager
{
    static dispatch_once_t once;
    static UniWebViewManager *instance;
    dispatch_once(&once, ^ { instance = [[UniWebViewManager alloc] init]; });
    return instance;
}

-(instancetype) init {
    self = [super init];
    if (self) {
        _webViewDic = [[NSMutableDictionary alloc] init];
    }
    return self;
}

-(void) addManagedWebView:(UniWebView *)webView forName:(NSString *)name
{
    if (![_webViewDic objectForKey:name]) {
        [_webViewDic setObject:webView forKey:name];
        webView.webViewId = webviewId;
        webviewId += 1;
        
        webView.messageSchemes = [NSMutableArray arrayWithObject:@"uniwebview"];
    } else {
//        NSLog(@"Duplicated name. Something goes wrong: %@", name);
    }
}

-(void) addManagedWebViewName:(NSString *)name insets:(NSEdgeInsets)insets screenSize:(CGSize)size
{
    UniWebView *webView = [[UniWebView alloc] init];
//    NSLog(@"addManagedWebViewName %@",webView);
    [webView setPolicyDelegate:self];
    [webView setFrameLoadDelegate:self];
    
    if (self.customUserAgent.length != 0) {
        [webView setCustomUserAgent:self.customUserAgent];
    }
    
    [self changeWebView:webView insets:insets screenSize:size];
    webView.hidden = YES;
    [self addManagedWebView:webView forName:name];
}

-(UniWebView *) getWebViewName:(nonnull NSString *)name {
    return _webViewDic[name];
}

-(void) changeWebViewName:(NSString *)name insets:(NSEdgeInsets)insets screenSize:(CGSize)size
{
    UniWebView *webView = [_webViewDic objectForKey:name];
    [self changeWebView:webView insets:insets screenSize:size];
}

-(void) changeWebView:(UniWebView *)webView insets:(NSEdgeInsets)insets screenSize:(CGSize)size
{
    NSRect f = NSMakeRect(0, 0, size.width - insets.left - insets.right, size.height - insets.top - insets.bottom);
    webView.frame = f;
}

-(void) webviewName:(NSString *)name beginLoadURL:(NSString *)urlString
{
    UniWebView *webView = [_webViewDic objectForKey:name];
//    NSLog(@"%@ beginLoadURL %@",webView, urlString);
    NSURL *url = [NSURL URLWithString:urlString];
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    
    for (NSString *key in webView.headers.allKeys) {
        [request setValue:webView.headers[key] forHTTPHeaderField:key];
    }
    
    [webView.mainFrame loadRequest:request];
}

-(void) webViewNameReload:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    [webView reload:self];
}

-(void) webViewNameStop:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    [webView stopLoading:self];
}

-(void) webViewNameCleanCache:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    [[NSURLCache sharedURLCache] removeCachedResponseForRequest:webView.mainFrame.dataSource.request];
}

-(void) webViewNameCleanCookie:(NSString *)name forKey:(NSString *)key {
    NSHTTPCookie *cookie;
    NSHTTPCookieStorage *cookieJar = [NSHTTPCookieStorage sharedHTTPCookieStorage];
    
    if (key.length) {
        NSLog(@"Removing cookie for %@", key);
        for (cookie in [cookieJar cookies]) {
            if ([cookie.name isEqualToString:key]) {
                [cookieJar deleteCookie:cookie];
                NSLog(@"Found cookie for %@, removed.", key);
            }
        }
    } else {
        NSLog(@"Removing all cookies");
        for (cookie in [cookieJar cookies]) {
            [cookieJar deleteCookie:cookie];
        }
    }

    [[NSUserDefaults standardUserDefaults] synchronize];
}

-(void) webViewName:(NSString *)name show:(BOOL)show fade:(BOOL)fade direction:(UniWebViewTransitionEdge)direction duration:(float)duration {
    WebView *webView = [_webViewDic objectForKey:name];
//    NSLog(@"%@ show %d",webView, show);
    webView.hidden = !show;
    
    if (fade || direction != UniWebViewTransitionEdgeNone) {
        [NSTimer scheduledTimerWithTimeInterval:duration
                                         target:[NSBlockOperation blockOperationWithBlock:^{
                                                    _CallMethod([name UTF8String], show ? "ShowTransitionFinished" : "HideTransitionFinished", "");}]
                                       selector:@selector(main)
                                       userInfo:nil
                                        repeats:NO];
    } else {
        dispatch_async(dispatch_get_main_queue(), ^{
            _CallMethod([name UTF8String], show ? "ShowTransitionFinished" : "HideTransitionFinished", "");
        });
    }
}

-(void) removeWebViewName:(NSString *)name {
//    NSLog(@"removeWebViewName %@",name);
    [_webViewDic removeObjectForKey:name];
}


-(void) updateBackgroundWebViewName:(NSString *)name transparent:(BOOL)transparent {
    WebView *webView = [_webViewDic objectForKey:name];
    [webView setDrawsBackground:!transparent];
}

-(void) setWebViewBackgroundColorName:(NSString *)name color:(NSColor *)color {
    BOOL transparent = (color.alphaComponent != 1.0);
    [self updateBackgroundWebViewName:name transparent:transparent];
}


-(bool) canGoBackWebViewName:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    return [webView canGoBack] ? true : false;
}

-(bool) canGoForwardWebViewName:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    return [webView canGoForward] ? true : false;
}

-(void) goBackWebViewName:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    [webView goBack];
}

-(void) goForwardWebViewName:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    [webView goForward];
}

-(void) webViewName:(NSString *)name loadHTMLString:(NSString *)htmlString baseURLString:(NSString *)baseURL {
    WebView *webView = [_webViewDic objectForKey:name];
    [webView.mainFrame loadHTMLString:htmlString baseURL:[NSURL URLWithString:baseURL]];
}

-(void) webViewName:(NSString *)name EvaluatingJavaScript:(NSString *)javaScript shouldCallBack:(BOOL)callBack {
    WebView *webView = [_webViewDic objectForKey:name];
    NSString *result = [webView stringByEvaluatingJavaScriptFromString:javaScript];
    if (callBack) {
        _CallMethod([name UTF8String], "EvalJavaScriptFinished", [result UTF8String]);
    }
}

-(NSString *) webViewGetUserAgent:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    return [webView stringByEvaluatingJavaScriptFromString:@"window.navigator.userAgent"];
}

-(void) webViewSetUserAgent:(NSString *)userAgent {
    self.customUserAgent = userAgent;
}

-(NSString *) webViewNameGetCurrentUrl:(NSString *)name {
    WebView *webView = [_webViewDic objectForKey:name];
    return [webView mainFrameURL] ?: @"";
}

-(void) webViewName:(NSString *)name setValue:(NSString *)value forHeaderField:(NSString *)key {
    UniWebView *webView = [_webViewDic objectForKey:name];
    if (value.length == 0) {
        [webView.headers removeObjectForKey:key];
    } else if (key.length != 0) {
        webView.headers[key] = value;
    }
}

-(NSString *) webViewName:(WebView *)webView {
    NSString *webViewName = [[_webViewDic allKeysForObject:webView] lastObject];
    if (!webViewName) {
        NSLog(@"Did not find the webview: %@",webViewName);
    }
    return webViewName;
}

-(void) addWebViewName:(NSString *)name urlScheme:(NSString *)scheme {
    @synchronized(name) {
        UniWebView *webView = [_webViewDic objectForKey:name];
        if (![webView.messageSchemes containsObject:scheme]) {
            [webView.messageSchemes addObject:scheme];
        }
    }
}

-(void) removeWebViewName:(NSString *)name urlScheme:(NSString *)scheme {
    @synchronized(name) {
        UniWebView *webView = [_webViewDic objectForKey:name];
        if ([webView.messageSchemes containsObject:scheme]) {
            [webView.messageSchemes removeObject:scheme];
        }
    }
}

- (void)webView:(UniWebView *)webView decidePolicyForNavigationAction:(NSDictionary *)actionInformation request:(NSURLRequest *)request frame:(WebFrame *)frame decisionListener:(id<WebPolicyDecisionListener>)listener {
    
    __block BOOL canResponse = NO;
    [webView.messageSchemes enumerateObjectsUsingBlock:^(NSString *scheme, NSUInteger idx, BOOL *stop) {
        if ([[request.URL absoluteString] rangeOfString:[scheme stringByAppendingString:@"://"]].location == 0) {
            canResponse = YES;
            *stop = YES;
        }
    }];
    
    if (canResponse) {
        NSString *rawMessage = [NSString stringWithFormat:@"%@",request.URL];
        NSString *webViewName = [self webViewName:webView];
        _CallMethod([webViewName UTF8String], "ReceivedMessage", [rawMessage UTF8String]);
        [listener ignore];
    } else {
        
        NSUInteger actionType = [[actionInformation objectForKey:WebActionNavigationTypeKey] unsignedIntValue];
        if (actionType == WebNavigationTypeLinkClicked && webView.openLinkInExternalBrowser) {
            [[NSWorkspace sharedWorkspace] openURL:request.URL];
        } else {
            [listener use];
        }
    }
}

- (void)webView:(UniWebView *)webView didStartProvisionalLoadForFrame:(WebFrame *)frame {
    if (frame == webView.mainFrame) {
        NSString *webViewName = [self webViewName:webView];
        webView.currentURL = webView.mainFrameURL;
        _CallMethod([webViewName UTF8String], "LoadBegin",[webView.mainFrameURL UTF8String]);
    }
}

- (void)webView:(UniWebView *)sender didFailLoadWithError:(NSError *)error forFrame:(WebFrame *)frame {
    if (frame == sender.mainFrame) {
        NSString *webViewName = [self webViewName:sender];
        _CallMethod([webViewName UTF8String], "LoadComplete", [error.localizedDescription UTF8String]);
    }
}

- (void)webView:(WebView *)sender didFailProvisionalLoadWithError:(NSError *)error forFrame:(WebFrame *)frame {
    if (frame == sender.mainFrame) {
        NSString *webViewName = [self webViewName:sender];
        _CallMethod([webViewName UTF8String], "LoadComplete", [error.localizedDescription UTF8String]);
    }
}

- (void)webView:(WebView *)sender didFinishLoadForFrame:(WebFrame *)frame {
    if (frame == sender.mainFrame) {
//        NSLog(@"%@ didFinishLoadForFrame",sender);
        NSString *webViewName = [self webViewName:sender];
        _CallMethod([webViewName UTF8String], "LoadComplete", "");
    }
}

- (void)update:(int)x y:(int)y deltaY:(float)deltaY buttonDown:(BOOL)buttonDown buttonPress:(BOOL)buttonPress buttonRelease:(BOOL)buttonRelease keyPress:(BOOL)keyPress keyCode:(unsigned char)keyCode keyChars:(NSString *)keyChars textureId:(int)tId webViewName:(NSString *)name {
    UniWebView *webView = [_webViewDic objectForKey:name];
//    NSLog(@"update WebView %@",webView);
	NSView *view = [[[webView mainFrame] frameView] documentView];
	NSGraphicsContext *context = [NSGraphicsContext currentContext];
	NSEvent *event;
    
	if (buttonDown) {
		if (buttonPress) {
			event = [NSEvent mouseEventWithType:NSLeftMouseDown
                                       location:NSMakePoint(x, y) modifierFlags:kNilOptions
                                      timestamp:GetCurrentEventTime() windowNumber:0
                                        context:context eventNumber:kNilOptions clickCount:1 pressure:1.0];
			[view mouseDown:event];
		} else {
			event = [NSEvent mouseEventWithType:NSLeftMouseDragged
                                       location:NSMakePoint(x, y) modifierFlags:kNilOptions
                                      timestamp:GetCurrentEventTime() windowNumber:0
                                        context:context eventNumber:kNilOptions clickCount:0 pressure:1.0];
			[view mouseDragged:event];
		}
	} else if (buttonRelease) {
		event = [NSEvent mouseEventWithType:NSLeftMouseUp
                                   location:NSMakePoint(x, y) modifierFlags:kNilOptions
                                  timestamp:GetCurrentEventTime() windowNumber:0
                                    context:context eventNumber:kNilOptions clickCount:0 pressure:1.0];
		[view mouseUp:event];
	}
    
	if (keyPress) {
		event = [NSEvent keyEventWithType:NSKeyDown
                                 location:NSMakePoint(x, y) modifierFlags:kNilOptions
                                timestamp:GetCurrentEventTime() windowNumber:0
                                  context:context
                               characters:keyChars
              charactersIgnoringModifiers:keyChars
                                isARepeat:NO keyCode:(unsigned short)keyCode];
		[view keyDown:event];
	}
    
	if (deltaY != 0) {
		CGEventRef cgEvent = CGEventCreateScrollWheelEvent(NULL, kCGScrollEventUnitLine, 1, deltaY * 3, 0);
		NSEvent *scrollEvent = [NSEvent eventWithCGEvent:cgEvent];
		CFRelease(cgEvent);
		[view scrollWheel:scrollEvent];
	}
    
    if (webviewRenderCounter == REFRESH_COUNT) {
        NSBitmapImageRep *bitmap = [_webViewDic objectForKey:name];
        @synchronized(bitmap) {
            bitmap = [webView bitmapImageRepForCachingDisplayInRect:[webView visibleRect]];
            if (bitmap) {
                webView.bitmap = bitmap;
            }
            webView.textureId = tId;
            [webView cacheDisplayInRect:[webView visibleRect] toBitmapImageRep:bitmap];
        }
        webviewRenderCounter = 0;
    } else {
        webviewRenderCounter += 1;
    }

}

- (void)render:(NSString *)webViewName {
//    NSLog(@"render webview name: %@", webViewName);
    UniWebView *webView = [_webViewDic objectForKey:webViewName];
    NSBitmapImageRep *bitmap = webView.bitmap;
//    NSLog(@"render bitmap: %@", bitmap);
    int textureId = webView.textureId;
//    NSLog(@"render textureId: %d", textureId);
	@synchronized(bitmap) {
		if (bitmap) {
            NSInteger samplesPerPixel = [bitmap samplesPerPixel];
            int rowLength = 0;
            int unpackAlign = 0;
            glGetIntegerv(GL_UNPACK_ROW_LENGTH, &rowLength);
            glGetIntegerv(GL_UNPACK_ALIGNMENT, &unpackAlign);
            glPixelStorei(GL_UNPACK_ROW_LENGTH,
                          (int)[bitmap bytesPerRow] / samplesPerPixel);
            glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
            glBindTexture(GL_TEXTURE_2D, textureId);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            
            int w;
            int h;
            glGetTexLevelParameteriv (GL_TEXTURE_2D, 0, GL_TEXTURE_WIDTH, &w);
            glGetTexLevelParameteriv (GL_TEXTURE_2D, 0, GL_TEXTURE_HEIGHT, &h);

            if (![bitmap isPlanar]) {
                glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, w, h,
                                GL_RGBA, GL_UNSIGNED_BYTE, [bitmap bitmapData]);

            }
            glPixelStorei(GL_UNPACK_ROW_LENGTH, rowLength);
            glPixelStorei(GL_UNPACK_ALIGNMENT, unpackAlign);
        }
	}
}

-(void) receivedRenderEvent:(int)eventID {
//    NSLog(@"Receiving render event: %d", eventID);
    for (WebView *webView in [_webViewDic allValues]) {
        NSString *name = [self webViewName:webView];
        if (eventID == [self webViewIdForName:name]) {
            [self render:name];
        }
    }
}

-(UniWebView *) webViewWithName:(NSString *)name {
    return [_webViewDic objectForKey:name];
}

-(int) webViewIdForName:(NSString *)name {
    return [self webViewWithName:name].webViewId;
}

@end

// Helper method to create C string copy
NSString* UniWebViewMakeNSString (const char* string) {
	if (string) {
		return [NSString stringWithUTF8String: string];
    } else {
		return [NSString stringWithUTF8String: ""];
    }
}

char* UniWebViewMakeCString(NSString *str) {
    const char* string = [str UTF8String];
	if (string == NULL) {
		return NULL;
    }
    
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

extern "C" {
	void _UniWebViewInit(const char *name, int top, int left, int bottom, int right, int screenWidth, int screenHeight);
	void _UniWebViewChangeInsets(const char *name, int top, int left, int bottom, int right, int screenWidth, int screenHeight);
	void _UniWebViewLoad(const char *name, const char *url);
    void _UniWebViewReload(const char *name);
    void _UniWebViewStop(const char *name);
	void _UniWebViewShow(const char *name, bool fade, int direction, float duration);
	void _UniWebViewHide(const char *name, bool fade, int direction, float duration);
    void _UniWebViewCleanCache(const char *name);
    void _UniWebViewCleanCookie(const char *name, const char *key);
    void _UniWebViewDestroy(const char *name);
    void _UniWebViewTransparentBackground(const char *name, BOOL transparent);
    void _UniWebViewSetBackgroundColor(const char *name, float r, float g, float b, float a);
    bool _UniWebViewCanGoBack(const char *name);
    bool _UniWebViewCanGoForward(const char *name);
    void _UniWebViewGoBack(const char *name);
    void _UniWebViewGoForward(const char *name);
    void _UniWebViewLoadHTMLString(const char *name, const char *html, const char *baseUrl);
    
    void _UniWebViewInputEvent(const char *name, int x, int y, float deltaY,
                               BOOL buttonDown, BOOL buttonPress, BOOL buttonRelease,
                               BOOL keyPress, unsigned char keyCode, const char *keyChars, int textureId);
    void UnityRenderEvent(int eventID);
    int _UniWebViewGetId(const char *name);
    void _UniWebViewEvaluatingJavaScript(const char *name, const char *javascript, BOOL callback);
    const char * _UniWebViewGetCurrentUrl(const char *name);
    void _UniWebViewAddUrlScheme(const char *name, const char *scheme);
    void _UniWebViewRemoveUrlScheme(const char *name, const char *scheme);
    const char * _UniWebViewGetUserAgent(const char *name);
    void _UniWebViewSetUserAgent(const char *userAgent);
    int _UniWebViewScreenScale();
    void _UniWebViewSetHeaderField(const char *name, const char *key, const char *value);
    bool _UniWebViewGetOpenLinksInExternalBrowser(const char *name);
    void _UniWebViewSetOpenLinksInExternalBrowser(const char *name, BOOL value);
    
    UnityRenderingEvent UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API _UniWebViewGetRenderEventFunc();
}

void _UniWebViewInit(const char *name, int top, int left, int bottom, int right, int screenWidth, int screenHeight) {
    NSEdgeInsets insets = NSEdgeInsetsMake(top, left, bottom, right);
    CGSize size = CGSizeMake(screenWidth, screenHeight);
    [[UniWebViewManager sharedManager] addManagedWebViewName:UniWebViewMakeNSString(name)
                                                      insets:insets
                                                  screenSize:size];
}

void _UniWebViewChangeInsets(const char *name, int top, int left, int bottom, int right, int screenWidth, int screenHeight) {
    NSEdgeInsets insets = NSEdgeInsetsMake(top, left, bottom, right);
    CGSize size = CGSizeMake(screenWidth, screenHeight);
    [[UniWebViewManager sharedManager] changeWebViewName:UniWebViewMakeNSString(name)
                                                  insets:insets
                                              screenSize:size];
}

void _UniWebViewLoad(const char *name, const char *url) {
    [[UniWebViewManager sharedManager] webviewName:UniWebViewMakeNSString(name)
                                      beginLoadURL:UniWebViewMakeNSString(url)];
}

void _UniWebViewReload(const char *name) {
    [[UniWebViewManager sharedManager] webViewNameReload:UniWebViewMakeNSString(name)];
}

void _UniWebViewStop(const char *name) {
    [[UniWebViewManager sharedManager] webViewNameStop:UniWebViewMakeNSString(name)];
}

void _UniWebViewShow(const char *name, bool fade, int direction, float duration) {
    [[UniWebViewManager sharedManager] webViewName:UniWebViewMakeNSString(name)
                                              show:YES
                                              fade:fade
                                         direction:UniWebViewTransitionEdge(direction)
                                          duration:duration];
}

void _UniWebViewHide(const char *name, bool fade, int direction, float duration) {
    [[UniWebViewManager sharedManager] webViewName:UniWebViewMakeNSString(name)
                                              show:NO
                                              fade:YES
                                         direction:UniWebViewTransitionEdge(direction)
                                          duration:duration];

}

void _UniWebViewCleanCache(const char *name) {
    [[UniWebViewManager sharedManager] webViewNameCleanCache:UniWebViewMakeNSString(name)];
}

void _UniWebViewCleanCookie(const char *name, const char *key) {
    [[UniWebViewManager sharedManager] webViewNameCleanCookie:UniWebViewMakeNSString(name) forKey:UniWebViewMakeNSString(key)];
}

void _UniWebViewDestroy(const char *name) {
    [[UniWebViewManager sharedManager] removeWebViewName:UniWebViewMakeNSString(name)];
}

void _UniWebViewTransparentBackground(const char *name, BOOL transparent) {
    [[UniWebViewManager sharedManager] updateBackgroundWebViewName:UniWebViewMakeNSString(name) transparent:transparent];
}

void _UniWebViewSetBackgroundColor(const char *name, float r, float g, float b, float a) {
    NSColor *color = [NSColor colorWithRed:r green:g blue:b alpha:a];
    [[UniWebViewManager sharedManager] setWebViewBackgroundColorName:UniWebViewMakeNSString(name) color:color];
}

bool _UniWebViewCanGoBack(const char *name) {
    return [[UniWebViewManager sharedManager] canGoBackWebViewName:UniWebViewMakeNSString(name)];
}

bool _UniWebViewCanGoForward(const char *name) {
    return [[UniWebViewManager sharedManager] canGoForwardWebViewName:UniWebViewMakeNSString(name)];
}

void _UniWebViewGoBack(const char *name) {
    [[UniWebViewManager sharedManager] goBackWebViewName:UniWebViewMakeNSString(name)];
}

void _UniWebViewGoForward(const char *name) {
    [[UniWebViewManager sharedManager] goForwardWebViewName:UniWebViewMakeNSString(name)];
}

void _UniWebViewLoadHTMLString(const char *name, const char *html, const char *baseUrl) {
    [[UniWebViewManager sharedManager] webViewName:UniWebViewMakeNSString(name)
                                    loadHTMLString:UniWebViewMakeNSString(html)
                                     baseURLString:UniWebViewMakeNSString(baseUrl)];
}

void UnityRenderEvent(int eventID) {
    @autoreleasepool {
        [[UniWebViewManager sharedManager] receivedRenderEvent:eventID];
    }
}

static void UNITY_INTERFACE_API OnRenderEvent(int eventID)
{
    @autoreleasepool {
        [[UniWebViewManager sharedManager] receivedRenderEvent:eventID];
    }
}

void _UniWebViewInputEvent(const char *name, int x, int y, float deltaY,
                           BOOL buttonDown, BOOL buttonPress, BOOL buttonRelease,
                           BOOL keyPress, unsigned char keyCode, const char *keyChars, int textureId) {
    [[UniWebViewManager sharedManager] update:x y:y deltaY:deltaY buttonDown:buttonDown buttonPress:buttonPress buttonRelease:buttonRelease keyPress:keyPress keyCode:keyCode keyChars:UniWebViewMakeNSString(keyChars) textureId:textureId webViewName:UniWebViewMakeNSString(name)];
}

int _UniWebViewGetId(const char *name) {
    return [[UniWebViewManager sharedManager] webViewIdForName:UniWebViewMakeNSString(name)];
    
}

const char *_UniWebViewGetCurrentUrl(const char *name) {
    return UniWebViewMakeCString([[UniWebViewManager sharedManager] webViewNameGetCurrentUrl:UniWebViewMakeNSString(name)]);
}

void _UniWebViewEvaluatingJavaScript(const char *name, const char *javascript, BOOL callback) {
    NSString *webViewName = UniWebViewMakeNSString(name);
    NSString *jsString = UniWebViewMakeNSString(javascript);
//    NSLog(@"webViewName:%@, eval js:%@",webViewName,jsString);
    [[UniWebViewManager sharedManager] webViewName:webViewName EvaluatingJavaScript:jsString shouldCallBack:callback];
}

void _UniWebViewAddUrlScheme(const char *name, const char *scheme) {
    [[UniWebViewManager sharedManager] addWebViewName:UniWebViewMakeNSString(name)
                                            urlScheme:UniWebViewMakeNSString(scheme)];
}

void _UniWebViewRemoveUrlScheme(const char *name, const char *scheme) {
    [[UniWebViewManager sharedManager] removeWebViewName:UniWebViewMakeNSString(name)
                                               urlScheme:UniWebViewMakeNSString(scheme)];
}

const char * _UniWebViewGetUserAgent(const char *name) {
    return UniWebViewMakeCString([[UniWebViewManager sharedManager] webViewGetUserAgent:UniWebViewMakeNSString(name)]);
}

void _UniWebViewSetUserAgent(const char *userAgent) {
    [[UniWebViewManager sharedManager] webViewSetUserAgent:UniWebViewMakeNSString(userAgent)];
}

int _UniWebViewScreenScale() {
    return (int)[[NSScreen mainScreen] backingScaleFactor];
}

void _UniWebViewSetHeaderField(const char *name, const char *key, const char *value) {
    [[UniWebViewManager sharedManager] webViewName:UniWebViewMakeNSString(name)
                                          setValue:UniWebViewMakeNSString(value)
                                    forHeaderField:UniWebViewMakeNSString(key)
     ];
}

bool _UniWebViewGetOpenLinksInExternalBrowser(const char *name) {
    UniWebView *webView = [[UniWebViewManager sharedManager] webViewWithName:UniWebViewMakeNSString(name)];
    return webView.openLinkInExternalBrowser;
}

void _UniWebViewSetOpenLinksInExternalBrowser(const char *name, BOOL value) {
    UniWebView *webView = [[UniWebViewManager sharedManager] webViewWithName:UniWebViewMakeNSString(name)];
    webView.openLinkInExternalBrowser = value;
}

UnityRenderingEvent UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API _UniWebViewGetRenderEventFunc() {
    return OnRenderEvent;
}
