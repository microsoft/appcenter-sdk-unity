#import <Foundation/Foundation.h>

@interface MSException : NSObject

@property(nonatomic, copy) NSString *type;
@property(nonatomic, copy) NSString *message;
@property(nonatomic, copy) NSString *stackTrace;
@property(nonatomic) NSArray<MSException *> *innerExceptions;
@property(nonatomic, copy) NSString *wrapperSdkName;

@end
